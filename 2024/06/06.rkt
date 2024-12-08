#lang racket

; Globals -----------------------------------------------------------------------
(define data-path "06-sample.txt")

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
(define (points-between p1 p2)
  (let ([x1 (posn-x p1)]
        [y1 (posn-y p1)]
        [x2 (posn-x p2)]
        [y2 (posn-y p2)])
    (cond
      ; up/down
      [(equal? x1 x2)
       (let ([ys (in-range (min y1 y2) (max y1 y2))])
         (for/list ([y ys]) (posn x1 y)))]
      [(equal? y1 y2)
       (let ([xs (in-range (min x1 x2) (max x1 x2))])
         (for/list ([x xs]) (posn x y1)))]
      [else (error (format "points not in line: ~a ~a" p1 p2))])))
               
    

; Obstacle functions ------------------------------------------------------------

(define (get-next-obstacle-pos p dir obstacles)
  (let ([facing (set-filter obstacles (curry headed-toward? p dir))])
    (if (set-empty? facing)
        #f
        (match dir
          ['up    (set-argmax posn-y facing)]
          ['down  (set-argmin posn-y facing)]
          ['left  (set-argmax posn-x facing)]
          ['right (set-argmin posn-x facing)]))))

(define (get-next-boundary-pos p dir bounds)
  (match dir
    ['up    (posn (posn-x p) -1)]
    ['down  (posn (posn-x p) (add1 (posn-y bounds)))]
    ['left  (posn -1 (posn-y p))]
    ['right (posn (add1 (posn-x bounds)) (posn-y p))]))


(define (build-path p dir bounds obstacles [seen-obstacles (set)] [path-taken '()])
  (if (in-bounds p bounds)
      (let ([next-obs (get-next-obstacle-pos p dir obstacles)])
        (if next-obs
            (let* ([opp-dir (turn-around dir)]
                   [p* (move next-obs opp-dir)]
                   [dir* (turn-right dir)]
                   [points (points-between p p*)]
                   [path-taken* (append points path-taken)])
              (if (set-member? seen-obstacles (list next-obs dir))
                  ; we're in a loop, let's add the first member as the last one and return
                  path-taken
                  ; we're still running around
                  (build-path p*
                              dir*
                              bounds
                              obstacles
                              (set-add seen-obstacles (list next-obs dir))
                              path-taken*)))
            (let* ([boundary-pos (get-next-boundary-pos p dir bounds)]
                   [points (points-between p boundary-pos)])
              (append points path-taken))))
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
#|
(define (ends-in-bounds? points)
  (in-bounds (last points)))

(let* ([original-obstacles (get-all-obstacles start 'up bounds obstacles)]
       [path-definitions (visited-points start our-obstacles)]
       [path-points (unique-points path-definitions)])
  (for/list
|#
        
