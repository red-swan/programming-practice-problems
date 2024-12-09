#lang racket

; Globals -----------------------------------------------------------------------
(define data-path "06.txt")

; Generic functions -------------------------------------------------------------
(define (between x a b)
  (and (<= a x) (<= x b)))

(define (set-filter st pred)
  (for/set ([s st]
            #:when (pred s))
    s))

(define (set-argmax proc st)
  (for/fold ([m #f])
            ([s st])
    (if (and m (< (proc s) (proc m)))
        m
        s)))

(define (set-argmin proc st)
  (for/fold ([m #f])
            ([s st])
    (if (and m (< (proc m) (proc s)))
        m
        s)))


; Movement functions ------------------------------------------------------------
(struct posn (x y) #:transparent)

(define (move p dir)
  (let ([x (posn-x p)]
        [y (posn-y p)])
    (match dir
      ['up    (posn x (sub1 y))]
      ['down  (posn x (add1 y))]
      ['left  (posn (sub1 x) y)]
      ['right (posn (add1 x) y)])))

(define (headed-toward? p1 dir p2)
  (let ([x1 (posn-x p1)]
        [y1 (posn-y p1)]
        [x2 (posn-x p2)]
        [y2 (posn-y p2)])
  (match dir
    ['up    (and (equal? x1 x2) (< y2 y1))]
    ['down  (and (equal? x1 x2) (< y1 y2))]
    ['left  (and (equal? y1 y2) (< x2 x1))]
    ['right (and (equal? y1 y2) (< x1 x2))])))

(define (turn-right dir)
  (match dir
    ['up 'right]
    ['right 'down]
    ['down 'left]
    ['left 'up]))

(define (turn-around dir)
  (match dir
    ['up 'down]
    ['left 'right]
    ['down 'up]
    ['right 'left]))

(define (in-bounds point maxes)
  (let ([x1 (posn-x point)]
        [y1 (posn-y point)]
        [x2 (posn-x maxes)]
        [y2 (posn-y maxes)])
    (and
     (between x1 0 x2)
     (between y1 0 y2))))

(define (distance-between p1 p2)
  (let ([x1 (posn-x p1)]
        [y1 (posn-y p1)]
        [x2 (posn-x p2)]
        [y2 (posn-y p2)])
    (+ (abs (- y2 y1)) (abs (- x2 x1)))))

; like in-range, inclusive left, exclusive right
(define (points-range p1 p2)
  (let ([x1 (posn-x p1)]
        [y1 (posn-y p1)]
        [x2 (posn-x p2)]
        [y2 (posn-y p2)])
    (cond
      [(equal? p1 p2) (list p1)]
      [(equal? x1 x2)
       (if (< y1 y2)
           (for/list ([y (in-range y1 y2)])    (posn x1 y))
           (for/list ([y (in-range y1 y2 -1)]) (posn x1 y)))]
      [(equal? y1 y2)
       (if (< x1 x2)
           (for/list ([x (in-range x1 x2)])    (posn x y1))
           (for/list ([x (in-range x1 x2 -1)]) (posn x y1)))]
      [else (error (format "points not in line: ~a ~a" p1 p2))])))
               
    

; Obstacle functions ------------------------------------------------------------

(define (get-first-pos-after-boundary p dir bounds)
  (let ([maxx1 (add1 (posn-x bounds))]
        [maxy1 (add1 (posn-y bounds))])
    (match dir
      ['up    (posn (posn-x p) -1)]
      ['down  (posn (posn-x p) maxy1)]
      ['left  (posn -1 (posn-y p))]
      ['right (posn maxx1 (posn-y p))])))

(define (get-next-obstacle-pos p dir obstacles)
  (let ([facing (set-filter obstacles (curry headed-toward? p dir))])
    (if (set-empty? facing)
        #f
        (match dir
          ['up    (set-argmax posn-y facing)]
          ['down  (set-argmin posn-y facing)]
          ['left  (set-argmax posn-x facing)]
          ['right (set-argmin posn-x facing)]))))

(define (get-next-stopping-point p dir bounds obstacles)
  (let ([next-obs (get-next-obstacle-pos p dir obstacles)])
    (if next-obs
        (move next-obs (turn-around dir))
        (get-first-pos-after-boundary p dir bounds))))


(define (build-path p dir bounds obstacles [seen-stopping-points (set (list p dir))] [path-taken '()])
  (if (in-bounds p bounds)
      (let ([p* (get-next-stopping-point p dir bounds obstacles)]
            [dir* (turn-right dir)])
        (if (set-member? seen-stopping-points (list p* dir*))
            (cons 'loop path-taken)
            (let* ([path (reverse (points-range p p*))]
                   [path-taken* (append path path-taken)]
                   [seen-stopping-points* (set-add seen-stopping-points (list p* dir*))])
              (build-path p* dir* bounds obstacles seen-stopping-points* path-taken*))))
      path-taken))


; Reading functions -------------------------------------------------------------
(define (extract-obstacle-positions str)
  (regexp-match-positions* #px"#" str #:match-select caar))

(define (build-position-set y xs)
  (for/set ([x xs])
    (posn x y)))

(define (extract-guard-position row str)
  (let ([match (regexp-match-positions #px"\\^" str)])
    (if match
        (posn (caar match) row)
        match)))

; Reading the data --------------------------------------------------------------
(define-values (obstacles start bounds)
  (for/fold ([obstacle-positions (set)]
             [initial-position #f]
             [max-x #f]
             [max-y #f]
             #:result (values obstacle-positions initial-position (posn max-x max-y)))
            ([line (in-lines (open-input-file data-path))]
             [y (in-naturals)])
    (let* ([xs (extract-obstacle-positions line)]
           [new-obstacle-positions (build-position-set y xs)]
           [max-x* (if max-x max-x (sub1 (string-length line)))]
           [max-y* y]
           [obstacle-positions* (set-union obstacle-positions new-obstacle-positions)]
           [initial-position* (if initial-position
                                  initial-position
                                  (extract-guard-position y line))])
      (values obstacle-positions*
              initial-position*
              max-x*
              max-y*))))





; Simulation Functions ----------------------------------------------------------

(define setup-path
  (remove-duplicates (build-path start 'up bounds obstacles)))
; Part 1 --------------------------------------------


(println (format "Part 1: ~a" (length setup-path)))

; Part 2 --------------------------------------------

(define (loops? points)
  (equal? 'loop (first points)))

(define new-obstacles
  (for/fold ([acc '()])
            ([candidate setup-path])
    (let* ([obstacles* (set-add obstacles candidate)]
           [new-path (build-path start 'up bounds obstacles*)])
      (if (loops? new-path)
          (cons candidate acc)
          acc))))
  
(println (format "Part 2: ~a" (length new-obstacles)))
      
