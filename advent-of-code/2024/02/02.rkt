#lang 2d racket

(require 2d/cond)

(define (sign x)
  (cond
    [(< x 0) 'negative]
    [(< 0 x) 'positive]
    [else    'zero]))

(define (between a b x)
  (and
   (<= a x)
   (<= x b)))

(define (safe-report? report [skip #f])
  (define (loop prev-x xs i prev-dir)
    (cond
      [(empty? xs) #t]
      [(and skip (= i skip)) (loop prev-x (rest xs) (add1 i) prev-dir)]
      [else (let* ([x (first xs)]
                   [diff (- x prev-x)]
                   [dir (sign diff)])
            #2dcond
  ╔══════════════════════════╦═══════════════════════╦═══════════════════════╦═════════════════╗
  ║                          ║   (not prev-dir)      ║ (equal? prev-dir dir) ║ else            ║
  ╠══════════════════════════╬═══════════════════════╩═══════════════════════╬═════════════════╣
  ║ (between 1 3 (abs diff)) ║             (loop x (rest xs) (add1 i) dir)   ║                 ║ 
  ╠══════════════════════════╬═══════════════════════════════════════════════╝                 ║
  ║ else                     ║                  #f                                             ║
  ╚══════════════════════════╩═════════════════════════════════════════════════════════════════╝ )]))
  (if (and skip (zero? skip))
      (loop (second report)  (rest (rest report)) 1 #f)
      (loop (first report) (rest report) 1 #f)))
  

         
(define (safe-report2? report)
  (define (loop [i 0] [max (length report)])
    (cond
      [(= i max) #f]
      [(safe-report? report i) #t]
      [else (loop (add1 i) max)]))
  (loop))

; Read Data ------------------------------------------------------------------

(define input-path (make-parameter "02-sample.txt"))
(input-path "02.txt")

(define (line->report line)
  (map string->number
       (string-split line " ")))

(define reports
  (with-input-from-file (input-path)
    (λ ()
      (for/list ([line (in-lines)])
        (line->report line)))))



; Part 1 --------------------------------------------------------------------

(count safe-report? reports)


; Part 2

(count safe-report2? reports)






























 