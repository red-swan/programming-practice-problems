#lang racket

; Globals -----------------------------------------------------------------------
;(define data-path "06-sample.txt")
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

(define (points-between p1 p2)
  (let ([x1 (posn-x p1)]
        [y1 (posn-y p1)]
        [x2 (posn-x p2)]
        [y2 (posn-y p2)])
    (cond
      ; up/down
      [(equal? x1 x2)
       (let ([ys (in-range (add1 (min y1 y2)) (max y1 y2))])
         (for/list ([y ys]) (posn x1 y)))]
      [(equal? y1 y2)
       (let ([xs (in-range (add1 (min x1 x2)) (max x1 x2))])
         (for/list ([x xs]) (posn x y1)))]
      [else (error "points not in line")])))
               
    

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


(define (get-all-obstacles p dir bounds obstacles [acc '()])
  (if (in-bounds p bounds)
      (let ([next-obs (get-next-obstacle-pos p dir obstacles)])
        (if next-obs
            (let* ([opp-dir (turn-around dir)]
                   [p* (move next-obs opp-dir)]
                   [dir* (turn-right dir)]
                   [acc* (cons next-obs acc)])
              (get-all-obstacles p* dir* bounds obstacles acc*))
            (reverse (cons (get-next-boundary-pos p dir bounds) acc))))
      (reverse acc)))



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


; Part 1 --------------------------------------------

(define (visited-points start obstacles)
  (for/fold ([visited `(,start)]
             [pos start]
             [dir 'up]
             #:result (reverse visited))
            ([obs obstacles])
    (let* ([pos* (move obs (turn-around dir))]
           [dir* (turn-right dir)]
           [visited* (cons pos* visited)])
      (values visited* pos* dir*))))

(define (unique-points points)
  (for/fold ([visited (apply set points)]
             [pos1 (first points)]
             #:result (set-count visited))
            ([pos2 (rest points)])
    (let* ([path-points (points-between pos1 pos2)]
           [visited* (set-union visited (list->set path-points))])
      (values visited* pos2))))

(define our-obstacles (get-all-obstacles start 'up bounds obstacles))
(unique-points (visited-points start our-obstacles))
            
#|
; Part 2 --------------------------------------------

        
(define (pos-before-hits p dir)
  (match dir
    ['up (move p 'down)]
    ['down (move p 'up)]
    ['left (move p 'right)]
    ['right (move p 'left)]))

(define (get-obstacle-loop obs-pos walker-pos dir)
  (define (loop pos dir acc)
    (if (equal? 4 (length acc))
        acc
        (let* ([last-obs (first acc)]
               [dir* (turn-right dir)]
               [next-obstacle (get-next-obstacle walker-pos dir*)])
          (if next-obstacle
              (let ([pos* (pos-before-hits next-obstacle dir*)])
                (loop pos* dir* (cons next-obstacle acc)))
              '()))))
  (loop walker-pos dir '(obs-pos)))

(define (loops? obs-pos walker-pos dir)
  (equal? 4 (get-obstacle-loop obs-pos walker-pos dir)))

(define (solve-part-2 traversed)
  (for/fold ([obstructions '()])
            ([step  traversed])
    (let* ([p (first step)]
           [dir (second step)]
           [candidate (move p dir)])
      (if (loops? candidate p dir)
          (cons candidate obstructions)
          obstructions))))

; Printing Solutions ------------------------------------------------------------
(displayln (format "Part 1: ~a" (solve-part-1 (run-game))))

;(displayln (format "Part 2: ~a" (solve-part-2 (run-game))))
(displayln (format "Part 1: ~a" (solve-part-2 (run-game))))

|#





